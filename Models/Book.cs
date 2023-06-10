namespace PRACTICA2.Models;
using System.ComponentModel.DataAnnotations;

public class Book{
    [Key]
    public int Id { get; set;}
    public String? Title { get; set;}

    public String? Description { get; set;}

    public String? Code { get; set;}


}