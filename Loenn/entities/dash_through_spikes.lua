local spikeHelper = require("helpers.spikes")

local handlers = $("Up", "Down", "Left", "Right"):map((dir -> spikeHelper.createEntityHandler("NerdHelper/DashThroughSpikes" .. dir, dir:lower(), false, false, {"Kalobi/NerdHelper/dashthroughspike"})))
for k, handler in lpairs(handlers) do
    handler.placements[1].name = "normal_spike"
    handler.placements[1].data = ($(handler.placements[1].data) .. $({red_boosters_count_as_dash = true, invert = false, zero_speed_only = false, along = true, into = true, diag = true}))()
    handler.placements[2] = {name = "fixer", data = table.shallowcopy(handler.placements[1].data)}
    handler.placements[2].data.type = "default"
    handler.placements[2].data.zero_speed_only = true
    handler.fieldInformation["type"].options = table.shallowcopy(spikeHelper.spikeVariants)
    table.insert(handler.fieldInformation["type"].options, "Kalobi/NerdHelper/dashthroughspike")
    handler.fieldInformation["type"].editable = true
    handler.fieldOrder = {"x", "y", (handler.placements[1].data["height"] ? "height" : "width"), "type", "red_boosters_count_as_dash", "along", "into", "diag", "zero_speed_only", "invert"}
end
return handlers()
