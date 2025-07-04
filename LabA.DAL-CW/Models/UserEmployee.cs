﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab_A.Abstraction.IModels;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.DAL.Models;

[Table("UserEmployee")]
public partial class UserEmployee : IUserEmployee
{
    [Key]
    [Column("user_employee_id")]
    public int UserEmployeeId { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; }

    [Column("password_hash")]
    [MaxLength(64)]
    public byte[] PasswordHash { get; set; }

    [Column("password_salt")]
    [MaxLength(128)]
    public byte[] PasswordSalt { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("UserEmployees")]
    public virtual Employee Employee { get; set; }


    IEmployee IUserEmployee.Employee
    {
        get => Employee;
        set => Employee = (Employee)value;
    }
}