using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class ExpressionTransformer : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ConstantExpression> _parameterReplacements;

    public ExpressionTransformer(Dictionary<ParameterExpression, ConstantExpression> parameterReplacements)
    {
        _parameterReplacements = parameterReplacements;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        // Check for + 1 and - 1 transformations
        if (node.NodeType == ExpressionType.Add && node.Right is ConstantExpression rightConstant && (int)rightConstant.Value == 1)
        {
            if (_parameterReplacements.TryGetValue((ParameterExpression)node.Left, out ConstantExpression replacement))
            {
                return Expression.Increment(replacement);
            }
        }
        else if (node.NodeType == ExpressionType.Subtract && node.Right is ConstantExpression rightConstantSub && (int)rightConstantSub.Value == 1)
        {
            if (_parameterReplacements.TryGetValue((ParameterExpression)node.Left, out ConstantExpression replacement))
            {
                return Expression.Decrement(replacement);
            }
        }

        return base.VisitBinary(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        // Replace parameters with constants if they exist in the replacements dictionary
        if (_parameterReplacements.TryGetValue(node, out ConstantExpression replacement))
        {
            return replacement;
        }

        return base.VisitParameter(node);
    }

    public static Expression Transform(Expression sourceExpression, Dictionary<ParameterExpression, ConstantExpression> parameterReplacements)
    {
        var transformer = new ExpressionTransformer(parameterReplacements);
        return transformer.Visit(sourceExpression);
    }
}
