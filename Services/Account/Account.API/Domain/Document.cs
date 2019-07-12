using System.Collections.Generic;
using ServiceSeed.Actors;

namespace Account.API.Domain
{
    public sealed class Document : ValueObject
    {
        public string Text { get; private set; }
        public string Photo { get; private set; }
        public bool Verified { get; private set; }

        public Document(string text)
        {
            Text = text;
            Verified = false;
        }

        public void AddPhoto(string photo)
        {
            Photo = photo;
        }

        public void Verify()
        {
            Verified = true;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
        }
    }
}