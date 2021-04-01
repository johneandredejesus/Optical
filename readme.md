# Optical

Projeto com a finalidade de cálculo de atenuação de potência da fibra óptica nos passivos de rede.

# Uso:

using Optical;
using Optical.Math;

ICalculationManager managercalculate = new CalculationManager(); **Objeto responsável pelo calculo do sistema**
 
OLT oLT = new OLT(1, managercalculate, 1); **Representa a OLT, de onde parte o sinal do sistema.**
 
ISplitter splitter = new BalancedSplitter(BalancedType._1_TO_2, managercalculate); **Splitter onde este represeta o atenuador do sistema.**
 
splitter.OutPutFiber[0].Size = 1; **Tamanho de fibra em km.**
 
splitter.AddInputFiber(oLT.OutPutFiber[0]); **Adiciona a fibra ao passivo de rede.**
 
ISplice splice = new Splice(SpliceType.FUSION, managercalculate); **Splice representa a emenda, caso haja a necessidade.**
 
splice.AddInPutFiber(splitter.OutPutFiber[0]); **Adiciona a fibra ao passivo de rede.**

ISplitter splitter2 = new UnBalancedSplitter(UnBalanced._10X90, managercalculate);  **Splitter onde este represeta o atenuador do sistema.**

splitter2.AddInputFiber(splice.OutPutFiber); **Adiciona a fibra ao passivo de rede.**

Console.WriteLine(managercalculate.TotalSystemLoss()); **Exibe a perda total do sistema.**
