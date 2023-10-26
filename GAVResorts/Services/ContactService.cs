using GAVResorts.Models;
using GAVResorts.Utils;

namespace GAVResorts.Services
{
    public class ContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public Contact Create(Contact contact)
        {
            if(_context.Contacts.Any(c => c.Telefone == contact.Telefone))
            {
                throw new Exception("Telefone já cadastrado para um usuário.");
            } else if (_context.Contacts.Any(c => c.Email == contact.Email))
            {
                throw new Exception("Email já cadastrado para um usuário");
            }

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        public Contact UpdateContact(Contact updatedContact)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == updatedContact.Id);
            if (contact == null)
            {
                return null;
            }

            contact.Nome = updatedContact.Nome;
            contact.Email = updatedContact.Email;
            contact.Telefone = updatedContact.Telefone;

            _context.Contacts.Update(contact);
            _context.SaveChanges();

            return contact;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _context.Contacts.ToList();
        }

        public Contact GetContactById(int id)
        {
            return _context.Contacts.FirstOrDefault(c => c.Id == id);
        }

        public bool Delete(int id)
        {
           var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
                return false;

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return true;

        }

       
    }
}
