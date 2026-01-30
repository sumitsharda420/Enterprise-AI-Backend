namespace DotNetAiChat.Memory
{
    public enum ConversationRole
    {
        System,
        User,
        Assistant
    }

    public record ConversationItem(
        ConversationRole Role,
        string Content
    );
}
