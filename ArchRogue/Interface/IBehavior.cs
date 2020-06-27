using ArchRogue.Core;
using ArchRogue.Systems;

namespace ArchRogue.Interface
{
    public interface IBehavior
    {
        bool Act(Monster monster, CommandSystem commandSystem);
    }
}
