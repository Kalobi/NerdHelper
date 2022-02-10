module NerdHelperBouncyJellyfish

using ..Ahorn, Maple

@mapdef Entity "NerdHelper/BouncyJellyfish" BouncyJellyfish(x::Integer, y::Integer, bubble::Bool=false, tutorial::Bool=false)

const placements = Ahorn.PlacementDict(
    "Bouncy Jellyfish (Nerd Helper)" => Ahorn.EntityPlacement(
        BouncyJellyfish
    ),
    "Bouncy Jellyfish (Floating) (Nerd Helper)" => Ahorn.EntityPlacement(
        BouncyJellyfish,
        "point",
        Dict{String, Any}(
            "bubble" => true
        )
    )
)

sprite = "objects/glider/idle0"

function Ahorn.selection(entity::BouncyJellyfish)
    x, y = Ahorn.position(entity)

    return Ahorn.getSpriteRectangle(sprite, x, y)
end

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::BouncyJellyfish, room::Maple.Room)
    Ahorn.drawSprite(ctx, sprite, 0, 0)

    if get(entity, "bubble", false)
        curve = Ahorn.SimpleCurve((-7, -1), (7, -1), (0, -6))
        Ahorn.drawSimpleCurve(ctx, curve, (1.0, 1.0, 1.0, 1.0), thickness=1)
    end
end

end
