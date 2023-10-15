using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Common;

public class BulletPoint : IEntity<BulletPointId> {

    private string text = null!;
    public string Text {
        get => text;
        set => text = value.Validate("text");
    }

    // Common Entity Properties
    public BulletPointId Id { get; private set; } = BulletPointId.Generate();
    
    // Default Constructor Needed for Persistence
    private BulletPoint() {
        Text = null!;
    }

    public BulletPoint(string Text) {
        this.Text = Text;
    }

    // public static implicit operator BulletPoint(string str) => new(str);
}