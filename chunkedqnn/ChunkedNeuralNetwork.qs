namespace Quantum.ChunkedNeuralNetwork
{
    // Including the namespace Primitive gives access to basic operations such as the 
    // Hadamard gates, CNOT gates, etc. that are useful for defining circuits. The 
    // implementation of these operations is dependent on the targeted machine. 
    open Microsoft.Quantum.Primitive;

    // The canon namespace contains many useful library functions for creating 
    // larger circuits, combinators, and utility functions. The implementation of 
    // the operations in the canon is machine independent as they are built on 
    // top of the primitive operations. 
    open Microsoft.Quantum.Canon;

	open Microsoft.Quantum.Extensions.Math;
	open Microsoft.Quantum.Extensions.Convert;


}