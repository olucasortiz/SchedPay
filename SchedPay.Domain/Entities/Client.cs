using System;
using System.Collections.Generic;
using System.Text;

namespace SchedPay.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ContactInfo { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public DateTimeOffset? UpdatedAtUtc { get; private set; }

        public DateTimeOffset? DeletedAtUtc { get; private set; }

        public bool IsDeleted => DeletedAtUtc.HasValue;
        public Client(Guid id, string name, string contactInfo, DateTimeOffset createdAtUtc)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Client name is required");

            if (string.IsNullOrWhiteSpace(contactInfo))
                throw new ArgumentException("Contact info is required");
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
            CreatedAtUtc = DateTimeOffset.UtcNow;
        }

        public void SoftDelete(DateTimeOffset nowUtc)
        {
            if(IsDeleted)
                throw new InvalidOperationException("Client is already deleted");
            DeletedAtUtc = nowUtc;
            UpdatedAtUtc = nowUtc;
        }

        public void Update(string name, string contactInfo, DateTimeOffset nowUtc)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Cannot update a deleted client");
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Client name is required");
            if(string.IsNullOrWhiteSpace(contactInfo))
                throw new ArgumentException("Contact info is required");
            Name = name;
            ContactInfo = contactInfo;
            UpdatedAtUtc = nowUtc;
        }
    }
}
