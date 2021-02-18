using Scrapper.Domain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Scrapper.Domain.Extensions
{
    public class PersonComparer<T>: IEqualityComparer<T>
        where T: Person
    {
        public bool Equals([AllowNull] T firsPerson, [AllowNull] T secondPerson)
        {
            if (Object.ReferenceEquals(firsPerson, secondPerson)) 
                return true;

            if (Object.ReferenceEquals(firsPerson, null) || Object.ReferenceEquals(secondPerson, null))
                return false;

            return firsPerson.Url.Equals(secondPerson.Url);
        }

        public int GetHashCode([DisallowNull] T person)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(person, null)) 
                return 0;

            //Get hash code for the Url field if it is not null.
            int hashPersonUrl = person.Url == null ? 0 : person.Url.GetHashCode();

            return hashPersonUrl;
        }
    }
}
