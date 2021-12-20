local spikeHelper = require("helpers.spikes")
local logging = require("logging")

local handlers = $("Up", "Down", "Left", "Right"):map((dir -> spikeHelper.createEntityHandler("NerdHelper/DashThroughSpikes" .. dir, dir:lower(), false, false, {"Kalobi/NerdHelper/dashthroughspike"})))
for k, handler in lpairs(handlers) do
    handler.placements[1].name = handler.placements[1].name:match("%a*")
    handler.placements[1].data = ($(handler.placements[1].data) .. $({red_boosters_count_as_dash = true, invert = false, zero_speed_only = false}))()
    handler.fieldOrder = {"x", "y", (handler.placements[1].data["height"] ? "height" : "width"), "type", "red_boosters_count_as_dash", "zero_speed_only", "invert"}
end
return handlers()
