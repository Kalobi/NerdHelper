for room in loadedState.side.map.rooms
	for entity in room.entities
        if startswith(entity.name, "NerdHelper/DashThroughSpikes") && haskey(entity.data, "textures")
            entity.data["type"] = entity.data["textures"]
            delete!(entity.data, "textures")
        end
    end
end