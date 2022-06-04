using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// COMPOSITE PATTERN
// Treat individual and aggregate objects identically
// NEURAL NET EXAMPLE

namespace DesignPatterns
{
    public static class ExtensionMethods
    {
        // generalised algorithm for single and composite values
        public static void ConnectTo(this IEnumerable<Neuron> src, IEnumerable<Neuron> dst)
        {
            if (ReferenceEquals(src, dst)) return;
            
            // connect each element of the source to each element in the destination
            foreach (var s in src)
            {
                foreach (var d in dst)
                {
                    s.Out.Add(d);
                    d.In.Add(s);
                }
            }
        }
    }
    
    // treat a neuron as a collection of just one neuron
    public class Neuron : IEnumerable<Neuron>
    {
        private float val;
        // connections from and to other neurons
        public List<Neuron> In, Out;

        public Neuron()
        {
            val = 0;
            In = new List<Neuron>();
            Out = new List<Neuron>();
        }
        
        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // A neuron layer is a collection of neurons
    public class NeuronLayer : Collection<Neuron>
    {
        public NeuronLayer(int count)
        {
            while (count-- > 0)
            {
                Add(new Neuron());
            }
        }
    }

    static class DriverCode
    {
        static void Main(string[] args)
        {
            // create neurons
            var n1 = new Neuron();
            var n2 = new Neuron();
            // create neuron layers
            var l1 = new NeuronLayer(10);
            var l2 = new NeuronLayer(3);
            
            // connect...
            // neuron to neuron
            n1.ConnectTo(n2);
            // neuron to layer
            n1.ConnectTo(l1);
            // layer to neuron
            l2.ConnectTo(n1);
            // layer to layer
            l1.ConnectTo(l2);
        }
    }
}