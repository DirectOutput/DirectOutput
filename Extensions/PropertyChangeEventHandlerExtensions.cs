using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;


namespace System.ComponentModel
{

    public static class PropertyChangeEventHandlerExtensions
    {
        //(((propertyExpression as LambdaExpression).Body as UnaryExpression).Operand as MemberExpression).Member.Name
        public static void Raise(this PropertyChangedEventHandler EventHandler, Expression<Func<object>> Expr)
        {
            if (EventHandler != null)
            {
                MemberExpression MemberExpr=null;
                if (Expr.Body is MemberExpression)
                {
                    MemberExpr = Expr.Body as MemberExpression;
                }
                else if(Expr.Body is UnaryExpression) 
                {
                    UnaryExpression UnaryExpr = Expr.Body as UnaryExpression;
                    if (UnaryExpr.Operand is MemberExpression)
                    {
                        MemberExpr = UnaryExpr.Operand as MemberExpression;
                    }
                }

                if (MemberExpr == null)
                {
                    throw new Exception("Could not raise PropertyChanged event. Could not find the MemberExpression in {0}.".Build(Expr.ToString()));
                }

                // Extract the right part (after "=&gt;")
                ConstantExpression vmExpression = MemberExpr.Expression as ConstantExpression;
                if (vmExpression == null)
                {
                    throw new ArgumentException("Could not raise PropertyChanged event. Could not find a Constant expression in part {0} of {1}".Build(MemberExpr.ToString(),MemberExpr.Expression.ToString()));
                }
                // Create a reference to the calling object to pass it as the sender
                LambdaExpression vmlambda = Expression.Lambda(vmExpression);
                Delegate vmFunc = vmlambda.Compile();
                object vm = vmFunc.DynamicInvoke();

                // Extract the name of the property to raise a change on
                string propertyName = MemberExpr.Member.Name;
                var e = new PropertyChangedEventArgs(propertyName);
                EventHandler(vm, e);
            }
           
           

        }
    }
}