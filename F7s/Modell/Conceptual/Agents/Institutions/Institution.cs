using F7s.Modell.Abstract;
using F7s.Modell.Conceptual.Agents.Roles;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual.Agents.Institutions
{
    // Basically a preset of roles. Groups should be able to institute insititutions in order to specialize. 
    // Insitutions may also contain some special logic, though it may be better to code that into agencies.
    // Examples of institutions might be as following:
    // Economics:
    // * Enterprises (already exist as unique class, might want to convert them to institutions) with sub-enterprises
    // * Work-Crew (Foreman, Workers)
    // Military:
    // * Command Hierarchy (commander, subordinate, recursive)
    // * Modern Infantry Squad (Leader, Gunner, Radioman, Grenadier, Riflemen)
    // * Boat Crew (Captain, Helmsman, Pilot, Navigator, Quartermaster, Hands)
    // * Sniper Team (Spotter, Sniper)
    // Social:
    // * Family (breadwinner, caretaker, care-recipients)
    // * Teaching (Teacher, Student) or Mentorship (Mentor, Mentee) or Apprenticeship (Master, Apprentice)
    // Political:
    // * Democracy (voters, representative)
    // * Monarchy (monarch, heir, subjects)
    // * Feudalism (lord, vassal, recursion)
    public abstract class Institution : GameEntity
    {
        public readonly List<Role> Roles;

        protected Institution()
        {
            Roles = GenerateRoles();
        }

        protected abstract List<Role> GenerateRoles();
    }
}
