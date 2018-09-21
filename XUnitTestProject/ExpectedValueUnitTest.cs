using System;
using Xunit;
using Xunit.Abstractions;
using QuantumNeuralNetwork;

namespace XUnitTestProject
{
    public class QNNChunkedMeasureEntanglementTests
    {
        [Fact]
        public void DriverTargets()
        {
            int timeChunks = 4;
            double time_final = 1.580 / (8 * Math.PI);
            int count = 1000;
            int totalEpochs = 11;
            int[] states = { 1, 2, 3, 4 };
            double[] target = { 1.0, 0, 0, 0.663325 };                 // There must be one target for every initial state

            CoupledTwoQubitQNN network = new CoupledTwoQubitQNN(timeChunks, time_final);

            for (int epoch = 1; epoch < totalEpochs; epoch++)
            {
                double[] entanglement = network.MeasureEntanglementWitness(states, count);

                for (int i = 0; i < timeChunks; i++)
                {
                    entanglement[i] = Math.Abs(entanglement[i]);

                    Assert.InRange(entanglement[i], 0, 1);
                }
            }
        }
    }
}
