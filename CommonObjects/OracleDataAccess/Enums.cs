using System;
using System.Collections.Generic;
using System.Text;

namespace OracleDataAccess
{
    public enum ParameterType
    {
        And,
        Or,
        Initial,
    }

    public enum TokenTypes
    {
        Equal,
        IsNull,
        IsNotNull,
        Like,
        LeftLike,
        RightLike,
        LessThanEqual,
        LessThan,
        GreaterThan,
        GreaterThanEqual,
        Unknown,
        In,
        NotIn,
        NotEqual,
    }

    public enum CaseType
    {
        Ignore,
        UpperCase,
        LowerCase,
        CaseSensitive,
    }
}
