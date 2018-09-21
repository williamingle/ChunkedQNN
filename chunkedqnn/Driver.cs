using System;

namespace QuantumNeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            int      timeChunks  = 4;
            double   time_final  = 1.580 / (8 * Math.PI);
            int      count       = 1000;
            int      totalEpochs = 11;
            int[]    states      = { 1, 2, 3, 4 };
            double[] target      = { 1.0, 0, 0, 0.663325 };                 // There must be one target for every initial state
            string   fmt         = " 0.0000000;-0.0000000";

            CoupledTwoQubitQNN network = new CoupledTwoQubitQNN(timeChunks, time_final);

            Console.WriteLine();
            Console.WriteLine("Entanglement:      Bell        Flat         C           P");
            Console.WriteLine("      Target:  " + target[0].ToString(fmt) + "  " +
                                                  target[1].ToString(fmt) + "  " +
                                                  target[2].ToString(fmt) + "  " +
                                                  target[3].ToString(fmt));

            for (int epoch = 1; epoch < totalEpochs; epoch++)
            {
                double[] entanglement = network.MeasureEntanglementWitness(states, count);

                for (int i = 0; i < timeChunks; i++)
                {
                    entanglement[i] = Math.Abs(entanglement[i]);
                }

                Console.WriteLine("     Witness:  " + entanglement[0].ToString(fmt) + "  " +
                                                      entanglement[1].ToString(fmt) + "  " +
                                                      entanglement[2].ToString(fmt) + "  " +
                                                      entanglement[3].ToString(fmt));
            }

            Console.WriteLine();
            Console.WriteLine("PRESS ANY KEY TO CONTINUE...");
            Console.ReadKey();
        }
    }
}