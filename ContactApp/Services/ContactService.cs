using System.Net;
using System.Net.Http.Json;
using Contact = ContactApp.Models.Contact;

namespace ContactApp.Services
{
    public class ContactService
    {
        private readonly HttpClient _httpClient;
        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Contact>>("contacts") ?? new List<Contact>();
        }
        public async Task<Contact?> GetContactByIdAsync(int id, CancellationToken ct = default)
        {
            var response = await _httpClient.GetAsync($"contacts/{id}", ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Contact>(ct) ?? new Contact();
        }
    }
}
