namespace Backend.Application.Common.Models;
public readonly record struct Unit
{
    // Die einzige Instanz dieses Objekts, die wir jemals brauchen
    public static readonly Unit Value = new(); 
}