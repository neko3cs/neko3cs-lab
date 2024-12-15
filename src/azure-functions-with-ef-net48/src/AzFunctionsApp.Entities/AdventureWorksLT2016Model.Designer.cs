﻿//------------------------------------------------------------------------------
// <auto-generated>
//    このコードはテンプレートから生成されました。
//
//    このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//    このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Runtime.Serialization;

[assembly: EdmSchema()]
namespace AzFunctionsApp.Entities
{
    #region コンテキスト

    /// <summary>
    /// 使用できるメタデータ ドキュメントはありません。
    /// </summary>
    public partial class AzFunctionsAppDatabaseContext : ObjectContext
    {
        #region コンストラクター
    
        /// <summary>
        /// アプリケーション構成ファイルの 'AzFunctionsAppDatabaseContext' セクションにある接続文字列を使用して新しい AzFunctionsAppDatabaseContext オブジェクトを初期化します。
        /// </summary>
        public AzFunctionsAppDatabaseContext() : base("name=AzFunctionsAppDatabaseContext", "AzFunctionsAppDatabaseContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 新しい AzFunctionsAppDatabaseContext オブジェクトを初期化します。
        /// </summary>
        public AzFunctionsAppDatabaseContext(string connectionString) : base(connectionString, "AzFunctionsAppDatabaseContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 新しい AzFunctionsAppDatabaseContext オブジェクトを初期化します。
        /// </summary>
        public AzFunctionsAppDatabaseContext(EntityConnection connection) : base(connection, "AzFunctionsAppDatabaseContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region 部分メソッド
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet プロパティ
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        public ObjectSet<Product> Product
        {
            get
            {
                if ((_Product == null))
                {
                    _Product = base.CreateObjectSet<Product>("Product");
                }
                return _Product;
            }
        }
        private ObjectSet<Product> _Product;

        #endregion

        #region AddTo メソッド
    
        /// <summary>
        /// Product EntitySet に新しいオブジェクトを追加するための非推奨のメソッドです。代わりに、関連付けられている ObjectSet&lt;T&gt; プロパティの .Add メソッドを使用してください。
        /// </summary>
        public void AddToProduct(Product product)
        {
            base.AddObject("Product", product);
        }

        #endregion

    }

    #endregion

    #region エンティティ
    
    /// <summary>
    /// 使用できるメタデータ ドキュメントはありません。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AdventureWorksLT2016Model", Name="Product")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Product : EntityObject
    {
        #region ファクトリ メソッド
    
        /// <summary>
        /// 新しい Product オブジェクトを作成します。
        /// </summary>
        /// <param name="productID">ProductID プロパティの初期値。</param>
        /// <param name="name">Name プロパティの初期値。</param>
        /// <param name="productNumber">ProductNumber プロパティの初期値。</param>
        /// <param name="standardCost">StandardCost プロパティの初期値。</param>
        /// <param name="listPrice">ListPrice プロパティの初期値。</param>
        /// <param name="sellStartDate">SellStartDate プロパティの初期値。</param>
        /// <param name="rowguid">rowguid プロパティの初期値。</param>
        /// <param name="modifiedDate">ModifiedDate プロパティの初期値。</param>
        public static Product CreateProduct(global::System.Int32 productID, global::System.String name, global::System.String productNumber, global::System.Decimal standardCost, global::System.Decimal listPrice, global::System.DateTime sellStartDate, global::System.Guid rowguid, global::System.DateTime modifiedDate)
        {
            Product product = new Product();
            product.ProductID = productID;
            product.Name = name;
            product.ProductNumber = productNumber;
            product.StandardCost = standardCost;
            product.ListPrice = listPrice;
            product.SellStartDate = sellStartDate;
            product.rowguid = rowguid;
            product.ModifiedDate = modifiedDate;
            return product;
        }

        #endregion

        #region 単純なプロパティ
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 ProductID
        {
            get
            {
                return _ProductID;
            }
            set
            {
                if (_ProductID != value)
                {
                    OnProductIDChanging(value);
                    ReportPropertyChanging("ProductID");
                    _ProductID = StructuralObject.SetValidValue(value, "ProductID");
                    ReportPropertyChanged("ProductID");
                    OnProductIDChanged();
                }
            }
        }
        private global::System.Int32 _ProductID;
        partial void OnProductIDChanging(global::System.Int32 value);
        partial void OnProductIDChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false, "Name");
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String ProductNumber
        {
            get
            {
                return _ProductNumber;
            }
            set
            {
                OnProductNumberChanging(value);
                ReportPropertyChanging("ProductNumber");
                _ProductNumber = StructuralObject.SetValidValue(value, false, "ProductNumber");
                ReportPropertyChanged("ProductNumber");
                OnProductNumberChanged();
            }
        }
        private global::System.String _ProductNumber;
        partial void OnProductNumberChanging(global::System.String value);
        partial void OnProductNumberChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Color
        {
            get
            {
                return _Color;
            }
            set
            {
                OnColorChanging(value);
                ReportPropertyChanging("Color");
                _Color = StructuralObject.SetValidValue(value, true, "Color");
                ReportPropertyChanged("Color");
                OnColorChanged();
            }
        }
        private global::System.String _Color;
        partial void OnColorChanging(global::System.String value);
        partial void OnColorChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Decimal StandardCost
        {
            get
            {
                return _StandardCost;
            }
            set
            {
                OnStandardCostChanging(value);
                ReportPropertyChanging("StandardCost");
                _StandardCost = StructuralObject.SetValidValue(value, "StandardCost");
                ReportPropertyChanged("StandardCost");
                OnStandardCostChanged();
            }
        }
        private global::System.Decimal _StandardCost;
        partial void OnStandardCostChanging(global::System.Decimal value);
        partial void OnStandardCostChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Decimal ListPrice
        {
            get
            {
                return _ListPrice;
            }
            set
            {
                OnListPriceChanging(value);
                ReportPropertyChanging("ListPrice");
                _ListPrice = StructuralObject.SetValidValue(value, "ListPrice");
                ReportPropertyChanged("ListPrice");
                OnListPriceChanged();
            }
        }
        private global::System.Decimal _ListPrice;
        partial void OnListPriceChanging(global::System.Decimal value);
        partial void OnListPriceChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Size
        {
            get
            {
                return _Size;
            }
            set
            {
                OnSizeChanging(value);
                ReportPropertyChanging("Size");
                _Size = StructuralObject.SetValidValue(value, true, "Size");
                ReportPropertyChanged("Size");
                OnSizeChanged();
            }
        }
        private global::System.String _Size;
        partial void OnSizeChanging(global::System.String value);
        partial void OnSizeChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Decimal> Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                OnWeightChanging(value);
                ReportPropertyChanging("Weight");
                _Weight = StructuralObject.SetValidValue(value, "Weight");
                ReportPropertyChanged("Weight");
                OnWeightChanged();
            }
        }
        private Nullable<global::System.Decimal> _Weight;
        partial void OnWeightChanging(Nullable<global::System.Decimal> value);
        partial void OnWeightChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> ProductCategoryID
        {
            get
            {
                return _ProductCategoryID;
            }
            set
            {
                OnProductCategoryIDChanging(value);
                ReportPropertyChanging("ProductCategoryID");
                _ProductCategoryID = StructuralObject.SetValidValue(value, "ProductCategoryID");
                ReportPropertyChanged("ProductCategoryID");
                OnProductCategoryIDChanged();
            }
        }
        private Nullable<global::System.Int32> _ProductCategoryID;
        partial void OnProductCategoryIDChanging(Nullable<global::System.Int32> value);
        partial void OnProductCategoryIDChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> ProductModelID
        {
            get
            {
                return _ProductModelID;
            }
            set
            {
                OnProductModelIDChanging(value);
                ReportPropertyChanging("ProductModelID");
                _ProductModelID = StructuralObject.SetValidValue(value, "ProductModelID");
                ReportPropertyChanged("ProductModelID");
                OnProductModelIDChanged();
            }
        }
        private Nullable<global::System.Int32> _ProductModelID;
        partial void OnProductModelIDChanging(Nullable<global::System.Int32> value);
        partial void OnProductModelIDChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime SellStartDate
        {
            get
            {
                return _SellStartDate;
            }
            set
            {
                OnSellStartDateChanging(value);
                ReportPropertyChanging("SellStartDate");
                _SellStartDate = StructuralObject.SetValidValue(value, "SellStartDate");
                ReportPropertyChanged("SellStartDate");
                OnSellStartDateChanged();
            }
        }
        private global::System.DateTime _SellStartDate;
        partial void OnSellStartDateChanging(global::System.DateTime value);
        partial void OnSellStartDateChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> SellEndDate
        {
            get
            {
                return _SellEndDate;
            }
            set
            {
                OnSellEndDateChanging(value);
                ReportPropertyChanging("SellEndDate");
                _SellEndDate = StructuralObject.SetValidValue(value, "SellEndDate");
                ReportPropertyChanged("SellEndDate");
                OnSellEndDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _SellEndDate;
        partial void OnSellEndDateChanging(Nullable<global::System.DateTime> value);
        partial void OnSellEndDateChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> DiscontinuedDate
        {
            get
            {
                return _DiscontinuedDate;
            }
            set
            {
                OnDiscontinuedDateChanging(value);
                ReportPropertyChanging("DiscontinuedDate");
                _DiscontinuedDate = StructuralObject.SetValidValue(value, "DiscontinuedDate");
                ReportPropertyChanged("DiscontinuedDate");
                OnDiscontinuedDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _DiscontinuedDate;
        partial void OnDiscontinuedDateChanging(Nullable<global::System.DateTime> value);
        partial void OnDiscontinuedDateChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.Byte[] ThumbNailPhoto
        {
            get
            {
                return StructuralObject.GetValidValue(_ThumbNailPhoto);
            }
            set
            {
                OnThumbNailPhotoChanging(value);
                ReportPropertyChanging("ThumbNailPhoto");
                _ThumbNailPhoto = StructuralObject.SetValidValue(value, true, "ThumbNailPhoto");
                ReportPropertyChanged("ThumbNailPhoto");
                OnThumbNailPhotoChanged();
            }
        }
        private global::System.Byte[] _ThumbNailPhoto;
        partial void OnThumbNailPhotoChanging(global::System.Byte[] value);
        partial void OnThumbNailPhotoChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String ThumbnailPhotoFileName
        {
            get
            {
                return _ThumbnailPhotoFileName;
            }
            set
            {
                OnThumbnailPhotoFileNameChanging(value);
                ReportPropertyChanging("ThumbnailPhotoFileName");
                _ThumbnailPhotoFileName = StructuralObject.SetValidValue(value, true, "ThumbnailPhotoFileName");
                ReportPropertyChanged("ThumbnailPhotoFileName");
                OnThumbnailPhotoFileNameChanged();
            }
        }
        private global::System.String _ThumbnailPhotoFileName;
        partial void OnThumbnailPhotoFileNameChanging(global::System.String value);
        partial void OnThumbnailPhotoFileNameChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid rowguid
        {
            get
            {
                return _rowguid;
            }
            set
            {
                OnrowguidChanging(value);
                ReportPropertyChanging("rowguid");
                _rowguid = StructuralObject.SetValidValue(value, "rowguid");
                ReportPropertyChanged("rowguid");
                OnrowguidChanged();
            }
        }
        private global::System.Guid _rowguid;
        partial void OnrowguidChanging(global::System.Guid value);
        partial void OnrowguidChanged();
    
        /// <summary>
        /// 使用できるメタデータ ドキュメントはありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime ModifiedDate
        {
            get
            {
                return _ModifiedDate;
            }
            set
            {
                OnModifiedDateChanging(value);
                ReportPropertyChanging("ModifiedDate");
                _ModifiedDate = StructuralObject.SetValidValue(value, "ModifiedDate");
                ReportPropertyChanged("ModifiedDate");
                OnModifiedDateChanged();
            }
        }
        private global::System.DateTime _ModifiedDate;
        partial void OnModifiedDateChanging(global::System.DateTime value);
        partial void OnModifiedDateChanged();

        #endregion

    }

    #endregion

}
