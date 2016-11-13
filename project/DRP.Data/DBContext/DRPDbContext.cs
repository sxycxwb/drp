/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using DRP.Code;

namespace DRP.Data
{
    public class DRPDbContext : DbContext
    {
        public DRPDbContext()
            : base(GetDRPDbConnection(),true)
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// 解密链接字符串
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetDRPDbConnection()
        {
            var providerName = ConfigurationManager.ConnectionStrings["DRPDbContext"].ProviderName;
            var conn = DbProviderFactories.GetFactory(providerName).CreateConnection();
            var connectString = ConfigurationManager.ConnectionStrings["DRPDbContext"].ConnectionString;
            conn.ConnectionString = DESEncrypt.Decrypt(connectString);
            return conn;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();//防止黑幕交易 要不然每次都要访问 
            Database.SetInitializer<DRPDbContext>(null);

            string assembleFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("DRP.Data.DLL", "DRP.Mapping.DLL").Replace("file:///", "");
            Assembly asm = Assembly.LoadFile(assembleFileName);
            var typesToRegister = asm.GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
