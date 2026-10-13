

namespace ContactApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Initials { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }


        public Contact(int id, string name, string phone)
        {
            Id = id;
            Initials = $"{name.Split(' ')[0][0]}{name.Split(' ')[1][0]}".ToUpper();
            Name = name;
            PhoneNumber = phone;
        }

        public Contact(int id, string name, string phone, string email): this(id, name, phone)
        {
            Email = email;
        }

        public Contact(Contact contact)
        {
            Id = contact.Id;
            Initials = contact.Initials;
            Name = contact.Name;
            PhoneNumber = contact.PhoneNumber;
            Email = contact.Email ?? string.Empty;
        }
    }
}
