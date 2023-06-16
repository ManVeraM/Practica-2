namespace PRACTICA2.Models;
using System.ComponentModel.DataAnnotations;

public class Dish{
    [Key]
    public int Id { get; set;}
    public String? Name { get; set;}

    public int Price { get; set;}

    


}