
namespace ContactApp.Models
{
    public static class ContactRepository
    {

        public static List<Contact> contacts = new List<Contact>
        {
            new(1, "John Doe", "809-111-2222"),
            new(2, "Samantha Adens", "809-111-3333"),
            new(3, "Maria Lopez", "809-111-4444", "maria.lopez@example.com"),
            new(4, "Tom Hanks", "809-111-5555", "tom.hanks@example.com"),
        };

        public static List<Contact> GetContacts()
        {
            return contacts;
        }

        public static Contact? GetContactById(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public static void UpdateContact(Contact contact)
        {
            var existingContact = GetContactById(contact.Id);
            if (existingContact != null)
            {
                existingContact.Name = contact.Name;
                existingContact.PhoneNumber = contact.PhoneNumber;
                existingContact.Email = contact.Email;
                existingContact.Initials = $"{contact.Name.Split(' ')[0][0]}{contact.Name.Split(' ')[1][0]}";
            }
        }

        public static int GetNextId()
        {
            return contacts.Count > 0 ? contacts.Max(c => c.Id) + 1 : 1;
        }

        public static void AddContact(Contact contact)
        {
            contacts.Add(contact);
        }

        public static void DeleteContact(int id)
        {
            var contactToDelete = GetContactById(id);
            if (contactToDelete != null)
            {
                contacts.Remove(contactToDelete);
            }
        }

        public static List<Contact> SearchContacts(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return GetContacts();
            }
            searchText = searchText.ToLower();
            var results =  contacts.Where(c => c.Name.ToLower().Contains(searchText) || c.PhoneNumber.Contains(searchText)).ToList();
            return results;
        }

    }
}
