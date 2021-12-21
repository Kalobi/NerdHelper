# replace all vanilla spikes with jumpthru fixer spikes (dash through spikes with "Zero Speed Only" checked)

local debugSpikeMap = Dict{String, String}(
    "spikesUp" => "NerdHelper/DashThroughSpikesUp",
    "spikesDown" => "NerdHelper/DashThroughSpikesDown",
    "spikesLeft" => "NerdHelper/DashThroughSpikesLeft",
    "spikesRight" => "NerdHelper/DashThroughSpikesRight"
)

local debugSizeKey = Dict{String, String}(
    "spikesUp" => "width",
    "spikesDown" => "width",
    "spikesLeft" => "height",
    "spikesRight" => "height"
)

for room in loadedState.side.map.rooms
	for entity in room.entities
		if entity.name in ("spikesUp", "spikesDown", "spikesLeft", "spikesRight")
            newSpike = Entity(debugSpikeMap[entity.name], Dict{String,Any}(
                "x" => entity.data["x"],
				"y" => entity.data["y"],
                debugSizeKey[entity.name] => entity.data[debugSizeKey[entity.name]],
                "type" => entity.data["type"],
                "zero_speed_only" => true,
                "red_boosters_count_as_dash" => true,
                "along" => true,
                "into" => true,
                "diag" => true,
                "invert" => false
            ), entity.id)
            push!(room.entities, newSpike)
        end
    end
    filter!(entity -> entity.name ∉ ("spikesUp", "spikesDown", "spikesLeft", "spikesRight"), room.entities)
end
