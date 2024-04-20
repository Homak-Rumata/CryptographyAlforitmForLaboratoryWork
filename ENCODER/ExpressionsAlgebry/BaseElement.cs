using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer = ENCODER.NumAlgoritm.SpecialInt;

namespace ENCODER.ExpressionsAlgebry
{
    interface IBaseElement
    { 

    }

    class BaseElement: IBaseElement
    {
        private BaseElement (int expression, Integer num )
        {
            _num = num;
            _expression = expression;
        }
        

        Integer _num;
        int _expression;

        public int GetExpression
        {
            get
            {
                return _expression;
            }
        }

        public int GetNum
        {
            get
            {
                return _expression;
            }
        }

        public static BaseElement operator + (BaseElement a, BaseElement b)
        {
            return new BaseElement(a._expression, a._num+b._num);
        }

        public static BaseElement operator * (BaseElement a, BaseElement b)
        {
            return new BaseElement(a._expression * b._expression, a._num * b._num);
        }



    }
}
