﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CurrencyInfo.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CurrencyEntitiesContext : DbContext
    {
        public CurrencyEntitiesContext()
            : base("name=CurrencyEntitiesContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbCurrency> tbCurrencies { get; set; }
        public virtual DbSet<vwExchangeRate> vwExchangeRates { get; set; }
    }
}