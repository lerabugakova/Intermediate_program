/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            ParameterExpression paramA = Expression.Parameter(typeof(int), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(int), "b");

            // Source expression: a + 1 - b
            Expression sourceExpression = Expression.Subtract(
                Expression.Add(paramA, Expression.Constant(1)),
                paramB
            );

            // Parameter replacements: a -> 5, b -> 3
            var parameterReplacements = new Dictionary<ParameterExpression, ConstantExpression>
        {
            { paramA, Expression.Constant(5) },
            { paramB, Expression.Constant(3) }
        };

            // Transform the expression
            Expression transformedExpression = ExpressionTransformer.Transform(sourceExpression, parameterReplacements);

            // Output the resulting expression tree
            Console.WriteLine(transformedExpression);

            // Compile and execute the transformed expression
            var lambda = Expression.Lambda<Func<int>>(transformedExpression);
            var compiledLambda = lambda.Compile();
            Console.WriteLine(compiledLambda());

            Console.ReadLine();
        }
    }
}
