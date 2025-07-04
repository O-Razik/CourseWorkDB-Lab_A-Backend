﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab_A.Abstraction.IModels;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.DAL.Models;

[Table("Status")]
public partial class Status : IStatus
{
    [Key]
    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("status", TypeName = "nvarchar(255)")]
    [StringLength(255)]
    public string StatusName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDatetime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDatetime { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<BiomaterialDelivery> BiomaterialDeliveries { get; set; } = new List<BiomaterialDelivery>();

    [InverseProperty("Status")]
    public virtual ICollection<ClientOrder> ClientOrders { get; set; } = new List<ClientOrder>();

    [InverseProperty("Status")]
    public virtual ICollection<InventoryDelivery> InventoryDeliveries { get; set; } = new List<InventoryDelivery>();

    [InverseProperty("Status")]
    public virtual ICollection<InventoryOrder> InventoryOrders { get; set; } = new List<InventoryOrder>();

    ICollection<IBiomaterialDelivery> IStatus.BiomaterialDeliveries
    {
        get => BiomaterialDeliveries.Cast<IBiomaterialDelivery>().ToList();
        set => BiomaterialDeliveries  = value.Cast<BiomaterialDelivery>().ToList();
    }

    ICollection<IClientOrder> IStatus.ClientOrders
    {
        get => ClientOrders.Cast<IClientOrder>().ToList();
        set => ClientOrders = value.Cast<ClientOrder>().ToList();
    }

    ICollection<IInventoryDelivery> IStatus.InventoryDeliveries
    {
        get => InventoryDeliveries.Cast<IInventoryDelivery>().ToList();
        set => InventoryDeliveries = value.Cast<InventoryDelivery>().ToList();
    }

    ICollection<IInventoryOrder> IStatus.InventoryOrders
    {
        get => InventoryOrders.Cast<IInventoryOrder>().ToList();
        set => InventoryOrders = value.Cast<InventoryOrder>().ToList();
    }
}