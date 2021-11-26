module NerdHelperDashThroughSpikes
using ..Ahorn, Maple

@mapdef Entity "NerdHelper/DashThroughSpikesUp" DashThroughSpikesUp(x::Integer, y::Integer, width::Integer=Maple.defaultSpikeWidth, red_boosters_count_as_dash::Bool=true, invert::Bool=false, textures::String="Kalobi/NerdHelper/dashthroughspike")
@mapdef Entity "NerdHelper/DashThroughSpikesDown" DashThroughSpikesDown(x::Integer, y::Integer, width::Integer=Maple.defaultSpikeWidth, red_boosters_count_as_dash::Bool=true, invert::Bool=false, textures::String="Kalobi/NerdHelper/dashthroughspike")
@mapdef Entity "NerdHelper/DashThroughSpikesLeft" DashThroughSpikesLeft(x::Integer, y::Integer, height::Integer=Maple.defaultSpikeHeight, red_boosters_count_as_dash::Bool=true, invert::Bool=false, textures::String="Kalobi/NerdHelper/dashthroughspike")
@mapdef Entity "NerdHelper/DashThroughSpikesRight" DashThroughSpikesRight(x::Integer, y::Integer, height::Integer=Maple.defaultSpikeHeight, red_boosters_count_as_dash::Bool=true, invert::Bool=false, textures::String="Kalobi/NerdHelper/dashthroughspike")

const entities = Dict{String, Type}(
    "up" => DashThroughSpikesUp,
    "down" => DashThroughSpikesDown,
    "left" => DashThroughSpikesLeft,
    "right" => DashThroughSpikesRight
)

const spikesUnion = Union{DashThroughSpikesUp, DashThroughSpikesDown, DashThroughSpikesLeft, DashThroughSpikesRight}

const placements = Ahorn.PlacementDict()
for (dir, entity) in entities
    key = "Dash Through Spikes ($(uppercasefirst(dir))) (Nerd Helper)"
    placements[key] = Ahorn.EntityPlacement(
        entity,
        "rectangle"
    )
end

Ahorn.editingOrder(entity::DashThroughSpikesUp) = String["x", "y", "width", "textures", "invert", "red_boosters_count_as_dash"]
Ahorn.editingOrder(entity::DashThroughSpikesDown) = String["x", "y", "width", "textures", "invert", "red_boosters_count_as_dash"]
Ahorn.editingOrder(entity::DashThroughSpikesLeft) = String["x", "y", "height", "textures", "invert", "red_boosters_count_as_dash"]
Ahorn.editingOrder(entity::DashThroughSpikesRight) = String["x", "y", "height", "textures", "invert", "red_boosters_count_as_dash"]

const directions = Dict{String, String}(
    "NerdHelper/DashThroughSpikesUp" => "up",
    "NerdHelper/DashThroughSpikesDown" => "down",
    "NerdHelper/DashThroughSpikesLeft" => "left",
    "NerdHelper/DashThroughSpikesRight" => "right"
)


const offsets = Dict{String, Tuple{Integer, Integer}}(
    "up" => (4, -4),
    "down" => (4, 4),
    "left" => (-4, 4),
    "right" => (4, 4)
)

const rotations = Dict{String, Number}(
    "up" => 0,
    "right" => pi / 2,
    "down" => pi,
    "left" => pi * 3 / 2
)

const resizeDirections = Dict{String, Tuple{Bool, Bool}}(
    "up" => (true, false),
    "down" => (true, false),
    "left" => (false, true),
    "right" => (false, true),
)

function Ahorn.renderSelectedAbs(ctx::Ahorn.Cairo.CairoContext, entity::spikesUnion)
    direction = get(directions, entity.name, "up")
    theta = rotations[direction] - pi / 2

    width = Int(get(entity.data, "width", 0))
    height = Int(get(entity.data, "height", 0))

    x, y = Ahorn.position(entity)
    cx, cy = x + floor(Int, width / 2) - 8 * (direction == "left"), y + floor(Int, height / 2) - 8 * (direction == "up")

    Ahorn.drawArrow(ctx, cx, cy, cx + cos(theta) * 24, cy + sin(theta) * 24, Ahorn.colors.selection_selected_fc, headLength=6)
end

function Ahorn.selection(entity::spikesUnion)
    if haskey(directions, entity.name)
        x, y = Ahorn.position(entity)

        width = Int(get(entity.data, "width", 8))
        height = Int(get(entity.data, "height", 8))

        direction = get(directions, entity.name, "up")
        variant = get(entity.data, "hotType", "default")

        width = Int(get(entity.data, "width", 8))
        height = Int(get(entity.data, "height", 8))

        ox, oy = offsets[direction]

        return Ahorn.Rectangle(x + ox - 4, y + oy - 4, width, height)
    end
end

Ahorn.minimumSize(entity::spikesUnion) = (8, 8)

function Ahorn.resizable(entity::spikesUnion)
    if haskey(directions, entity.name)
        direction = get(directions, entity.name, "up")
        return resizeDirections[direction]
    end
end


function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::spikesUnion)
    if haskey(directions, entity.name)
        direction = get(directions, entity.name, "up")

        width = get(entity.data, "width", 8)
        height = get(entity.data, "height", 8)

        for ox in 0:8:width - 8, oy in 0:8:height - 8
            drawX = ox + offsets[direction][1]
            drawY = oy + offsets[direction][2]

            Ahorn.drawSprite(ctx, "danger/spikes/Kalobi/NerdHelper/dashthroughspike_$(direction)00", drawX, drawY)
        end
    end
end

function Ahorn.flipped(entity::DashThroughSpikesUp, horizontal::Bool)
    if !horizontal
        return DashThroughSpikesDown(entity.x, entity.y, entity.width, entity.red_boosters_count_as_dash, entity.invert, entity.textures)
    end
end

function Ahorn.flipped(entity::DashThroughSpikesDown, horizontal::Bool)
    if !horizontal
        return DashThroughSpikesUp(entity.x, entity.y, entity.width, entity.red_boosters_count_as_dash, entity.invert, entity.textures)
    end
end

function Ahorn.flipped(entity::DashThroughSpikesLeft, horizontal::Bool)
    if horizontal
        return DashThroughSpikesRight(entity.x, entity.y, entity.height, entity.red_boosters_count_as_dash, entity.invert, entity.textures)
    end
end

function Ahorn.flipped(entity::DashThroughSpikesRight, horizontal::Bool)
    if horizontal
        return DashThroughSpikesLeft(entity.x, entity.y, entity.height, entity.red_boosters_count_as_dash, entity.invert, entity.textures)
    end
end

for (left, normal, right) in (t -> circshift([DashThroughSpikesUp, DashThroughSpikesRight, DashThroughSpikesDown, DashThroughSpikesLeft],t)).(0:3)
    @eval function Ahorn.rotated(entity::$normal, steps::Int)
            if steps > 0
                return Ahorn.rotated($right(entity.x, entity.y, typeof(entity) in (DashThroughSpikesUp, DashThroughSpikesDown) ? entity.width : entity.height, entity.red_boosters_count_as_dash, entity.invert, entity.textures), steps - 1)

            elseif steps < 0
                return Ahorn.rotated($left(entity.x, entity.y, typeof(entity) in (DashThroughSpikesUp, DashThroughSpikesDown) ? entity.width : entity.height, entity.red_boosters_count_as_dash, entity.invert, entity.textures), steps + 1)
            end

            return entity
        end
end

end
