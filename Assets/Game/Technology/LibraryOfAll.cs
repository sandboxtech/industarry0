﻿

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(Book), 100, 10)]
    public class LibraryOfAll : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;
        protected override Type TechnologyPointType => typeof(Book);
        protected override long TechnologyPointIncRequired => 1;
        protected override long TechnologyPointMaxRevenue => BaseCost;

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(LibraryOfAll), 0),

            (typeof(LibraryOfAgriculture), 1*BaseCost),
            (typeof(LibraryOfClothing), 1*BaseCost),

            (typeof(LibraryOfHandcraft),  2*BaseCost),
            (typeof(LibraryOfGeography),  3*BaseCost),
            (typeof(LibraryOfLogistics),  5*BaseCost),
            (typeof(LibraryOfEconomy),  5*BaseCost),
            (typeof(LibraryOfConstruction),  5*BaseCost),
            (typeof(LibraryOfMetalWorking),  8*BaseCost),

            (typeof(SchoolOfAll), 10*BaseCost)
        };
    }
}
