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
    
    public partial class Изделия
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Изделия()
        {
            this.Заказные_изделия = new HashSet<Заказные_изделия>();
            this.Фурнитура_изделия = new HashSet<Фурнитура_изделия>();
            this.Ткани = new HashSet<Ткани>();
        }
    
        public string Артикул { get; set; }
        public string Наименование { get; set; }
        public string Ширина { get; set; }
        public string Длина { get; set; }
        public string цена { get; set; }
        public byte[] Изображение { get; set; }
        public string коментарий { get; set; }
        public Nullable<int> Условия { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказные_изделия> Заказные_изделия { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Фурнитура_изделия> Фурнитура_изделия { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ткани> Ткани { get; set; }
        public virtual Спецификация Спецификация { get; set; }
    }
}
