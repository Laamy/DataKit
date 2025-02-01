namespace Datapack.Operators;

public class OperatorAction
{
    //gamemode: gamemode with ! operators (example !creative, creative)
    //level, limit: number (example 5)
    //name: string (example "UsernameHere")
    //nbt: complex nbt (example {..}
    //predicate: string (example "datapack:custom_predicate") (for datapacks only ig)
    //scores: nbt (example {right_click_test=1})
    //sort: SortType (example SortType.Nearest)
    //tag, team: string (example "team_name")
    //type - special operator for entity operations that require a specific entity type

    public virtual string ToRaw() => string.Empty;
}
