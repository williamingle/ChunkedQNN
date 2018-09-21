using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using System;

namespace Quantum.ChunkedQNN
{
    class Program
    {
        static void Main(string[] args)
        {
            // trained values, will be initial values after this is all working
            double[] ka_amp = { 2.489, 2.473, 2.485, 2.495 };
            double[] kb_amp = { 2.489, 2.473, 2.485, 2.495 };
            double[] ea_bias = { 0.0929, 0.116, 0.0954, 0.0833 };
            double[] eb_bias = { 0.0929, 0.116, 0.0954, 0.0833 };
            double[] z_coupling = { 0.0382, 0.128, 0.117, 0.0382 };
            double dt = 1.580/(8 * Math.PI);
            int count = 10000;
                       
            // subroutine to compute angles for the quantum operator returns a tuple (structure) of double[4] vectors
            // QnnAngles.(alpha_a, theta_a, alpha_b, theta_b, beta)
            var (alpha_a, theta_a, alpha_b, theta_b, beta) = GetAngles(ka_amp, ea_bias, kb_amp, eb_bias, z_coupling, dt);
            // prints QnnAngles to console, values match equivalent MATLAB computations
            Console.WriteLine("alpha_a           theta_a           alpha_b           theta_b           beta");
            for (int j = 0; j < 4; j++)
            {
                Console.WriteLine($"{alpha_a[j],0:F15} {theta_a[j],0:F15} {alpha_b[j],0:F15} {theta_b[j],0:F15} {beta[j],0:F15}");
            }

            // this is the hard part, invoking the quantum simulator and getting it to run
            // what's down here is an aborted attempt to modify the Bell starter project from the Q# website
            // It's all commented out so that the compiler doesn't trip over it and will check to see if ChunkedQNN.qs complies 

            // THIS IS WHERE THE PROBLEM IS
            using (var sim = new QuantumSimulator())
            {
                QArray<double> al_a = new QArray<double>(alpha_a);
                QArray<double> th_a = new QArray<double>(theta_a);
                QArray<double> al_b = new QArray<double>(alpha_b);
                QArray<double> th_b = new QArray<double>(theta_b);
                QArray<double> be   = new QArray<double>(beta);

                for (int n = 4; n < 5; n++) // n set to 4 just to run P state; all states set n = 1
                {
                    string statename;
                    switch (n)
                    {
                        case 1:
                            statename = "Bell";
                            break;
                        case 2:
                            statename = "Flat";
                            break;
                        case 3:
                            statename = "C";
                            break;
                        default:
                            statename = "P";
                            break;
                    }
                    var res = QNN.Run(sim, al_a, th_a, al_b, th_b, be, count, n).Result;
                    double witness = res;
                    double divisor = count;
                    Console.WriteLine();
                    double entanglement = Math.Pow(res / divisor, 2);
                    Console.WriteLine($"{statename}" + " entanglement");
                    Console.WriteLine($"{entanglement,0:F15}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("PRESS ANY KEY TO CONTINUE...");
            Console.ReadKey();
        }

        private static (double[] alpha_a, double[] theta_a, double[] alpha_b, double[] theta_b, double[] beta)
               GetAngles(double[] ka_amp, double[] ea_bias, double[] kb_amp, double[] eb_bias, double[] z_coupling, double dt)
        {
            // computes the angles using the amplitudes, biases, and qubit coupling parameters
            double[] alpha_a = new double[4];
            double[] theta_a = new double[4];
            double[] alpha_b = new double[4];
            double[] theta_b = new double[4];
            double[] beta    = new double[4];

            for (int i=0; i<4; i++)
            {
                alpha_a[i] = 4 * Math.PI * dt * Math.Sqrt(Math.Pow(ka_amp[i], 2) + Math.Pow(ea_bias[i], 2)); 
                theta_a[i] = Math.Asin(ka_amp[i] / Math.Sqrt(Math.Pow(ka_amp[i], 2) + Math.Pow(ea_bias[i], 2)));
                alpha_b[i] = 4 * Math.PI * dt * Math.Sqrt(Math.Pow(kb_amp[i], 2) + Math.Pow(eb_bias[i], 2)); 
                theta_b[i] = Math.Asin(kb_amp[i] / Math.Sqrt(Math.Pow(kb_amp[i], 2) + Math.Pow(eb_bias[i], 2)));
                beta[i]    = 4 * dt * Math.PI * z_coupling[i];                                                 
            }
            return (alpha_a, theta_a, alpha_b, theta_b, beta);
        }
    }
}