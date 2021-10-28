//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiSegura.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Habitacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Habitacion()
        {
            this.Reserva = new HashSet<Reserva>();
        }
    
        public int Codigo { get; set; }
        public int CodigoHotel { get; set; }
        public string Numero { get; set; }
        public int Capacidad { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
    
        public virtual Hotel Hotel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}