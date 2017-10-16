﻿namespace Genie.Core.Models.Abstract
{
    internal interface IEnumValue
    {
        string Name { get; }
        object Value { get; }
        string FieldName { get; }
    }
}