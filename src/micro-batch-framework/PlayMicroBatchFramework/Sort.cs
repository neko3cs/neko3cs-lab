using System;
using System.Collections.Generic;
using System.Linq;
using MicroBatchFramework;

namespace PlayMicroBatchFramework
{
    public class Sort : BatchBase
    {
        private List<int> Source { get; set; } = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                                                        .OrderBy(num => Guid.NewGuid())
                                                        .ToList();

        private void ShowSource()
        {
            Source.ForEach(num => Console.Write(num + ", "));
            Console.WriteLine();
        }

        public void BubbleSort()
        {
            ShowSource();

            for (var i = 0; i < Source.Count; i++)
            {
                for (var j = i + 1; j < Source.Count; j++)
                {
                    if (Source[i] > Source[j])
                    {
                        var tmp = Source[i];
                        Source[i] = Source[j];
                        Source[j] = tmp;
                    }
                }
            }

            ShowSource();
        }
    }
}
