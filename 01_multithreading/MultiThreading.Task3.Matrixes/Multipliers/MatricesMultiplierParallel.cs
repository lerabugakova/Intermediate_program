using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Drawing;
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            if (m1.ColCount != m2.RowCount)
            {
                throw new ArgumentException("Number of columns in matrixA must equal number of rows in matrixB.");
            }

            var result = new Matrix(m1.RowCount, m2.ColCount);

            Parallel.For(0, m1.ColCount, i =>
            {
                for (int j = 0; j < m2.ColCount; j++)
                {
                    long sum = 0;
                    for (int k = 0; k < m1.ColCount; k++)
                    {
                        sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                    }

                    result.SetElement(i, j, sum);
                }
            });

            return result;
        }
    }
}
