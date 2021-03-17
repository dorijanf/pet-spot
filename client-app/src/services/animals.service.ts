import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfig } from 'src/app/configuration/config';
import { AnimalBm } from 'src/app/generated-models/animal-bm';
import { AnimalDto } from 'src/app/generated-models/animal-dto';
import { LocationBm } from 'src/app/generated-models/location-bm';

@Injectable({
  providedIn: 'root'
})

export class AnimalsService {
  apiUrl: string;
  appUrl: string;

  /**
   * Service containing methods for animal object
   * creation, deletion, retrieval and modification.
   * @param http
   * @param config
   */
  constructor(private http: HttpClient, private config: AppConfig) {
    this.apiUrl = this.config.setting['PathAPI'];
    this.appUrl = 'api/Animals/'
  }

  /**
   * Creates a new animal object and stores it in the database
   * @param model - contains animal registration information
   * @returns
   */
  createAnimal(model: AnimalBm) {
    return this.http.post(this.apiUrl + this.appUrl, model);
  }

  /**
   * Gets a list of all animals for currently logged in user
   * @returns - a list of animals
   */
  getAnimals() : Observable<AnimalDto[]> {
    return this.http.get<AnimalDto[]>(this.apiUrl + this.appUrl);
  }

  /**
   * Gets an animal by its id
   * @param id - id of animal to be retrieved
   * @returns - animal object
   */
  getAnimal(id: number) : Observable<AnimalDto> {
    return this.http.get<AnimalDto>(this.apiUrl + this.appUrl + id);
  }

  /**
   * Deletes an animal by its id
   * @param id
   * @returns
   */
  deleteAnimal(id: number) {
    return this.http.delete(this.apiUrl + this.appUrl + id);
  }

  /**
   * Find an animal by its id and then updates it with
   * provided model object
   * @param id - id of animal to be updated
   * @param model - data to be updated
   * @returns
   */
  updateAnimal(id: number, model: AnimalBm) {
    return this.http.put(this.apiUrl + this.appUrl + id, model);
  }

  updateAnimalLocation(model: LocationBm){
    return this.http.patch(this.apiUrl + this.appUrl, model);
  }


}
