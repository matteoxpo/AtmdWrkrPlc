﻿namespace Domain.Entities.People;

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    
    public uint ID { get; private set; }

    public Address(string street, string city, string state, string zipCode, string country, uint id)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        ID = id;
    }
}