//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Фабрика_ткани_WPF_2._0
{
    using System;
    using System.Collections.Generic;
    
    public partial class Фурнитура
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Фурнитура()
        {
            this.Фурнитура_изделия = new HashSet<Фурнитура_изделия>();
            this.Заказ = new HashSet<Заказ>();
        }
    
        public string Артикул { get; set; }
        public string Наименование { get; set; }
        public string Ширина { get; set; }
        public string Длина { get; set; }
        public string Тип { get; set; }
        public string Цена { get; set; }
        public Nullable<decimal> Вес { get; set; }
        public byte[] Изобажение { get; set; }
    
        public virtual Склад_фунитуры Склад_фунитуры { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Фурнитура_изделия> Фурнитура_изделия { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказ> Заказ { get; set; }
    }
}
