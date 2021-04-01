# Optical

Projeto com a finalidade de cálculo de atenuação de potência da fibra óptica nos passivos de rede.

# Uso:

 using Optical;

 using Optical.Math;

**Objeto responsável pelo calculo do sistema:**

    ICalculationManager calculationManager  = new CalculationManager();
 
**Representa a OLT, de onde parte o sinal do sistema:**

    OLT oLT = new OLT(1, managercalculate, 1);
 
**Splitter onde este represeta o atenuador do sistema:**

    ISplitter splitter = new BalancedSplitter(BalancedType._1_TO_2, calculationManager); 

**Tamanho da fibra em km:**

    splitter.OutPutFiber[0].Size = 1; 
   
 **Adiciona a fibra ao passivo de rede:**
 
    splitter.AddInputFiber(oLT.OutPutFiber[0]);
    
**Splice representa a emenda, caso haja a necessidade:**

    ISplice splice = new Splice(SpliceType.FUSION, calculationManager); 

**Adiciona a fibra ao passivo de rede:**

    splice.AddInPutFiber(splitter.OutPutFiber[0]); 

**Splitter onde este represeta o atenuador do sistema:**

    ISplitter splitter2 = new UnBalancedSplitter(UnBalanced._10X90, calculationManager); 

**Adiciona a fibra ao passivo de rede:**

    splitter2.AddInputFiber(splice.OutPutFiber);

**Exibe a perda total do sistema:**

    Console.WriteLine(calculationManager.TotalSystemLoss());
