﻿using Genie.Core.Models.Abstract;

namespace Genie.Core.Models.Concrete
{
    internal class ReferenceList : IReferenceList
    {
        public string ReferencedRelationName { get; set; }
        public string ReferencedPropertyName { get; set; }
        public string ReferencedPropertyOnThisRelation { get; set; }
    }
}