local script = {}

script.name = "replace_spikes"
script.displayName = "Replace vanilla spikes with Jumpthru Fixer Spikes"

function script.run(room, args)
    for _, entity in ipairs(room.entities) do
        if string.find(entity._name, "spikes") == 1 then
           entity._name = "NerdHelper/DashThroughSpikes" .. string.sub(entity._name, 7)
           entity.red_boosters_count_as_dash = true
           entity.invert = false
           entity.zero_speed_only = true
           entity.along = true
           entity.into = true
           entity.diag = true
        end
    end
end

return script
