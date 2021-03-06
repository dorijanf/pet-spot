
// $Classes/Enums/Interfaces(filter)[template][separator]
// filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
// template: The template to repeat for each matched item
// separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

// More info: http://frhagn.github.io/Typewriter/


import { LocationBm } from './location-bm';
export class AnimalDto   {
    	public id: number;
	public name: string;
	public breed: string;
	public age: number;
	public description: string;
	public userId: string;
	public speciesId: number;
	public location: LocationBm;
    }

