import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AnimalDto } from 'src/app/generated-models/animal-dto';
import { SpeciesEnum } from 'src/app/generated-models/species-enum';
import { AnimalsService } from 'src/services/animals.service';

@Component({
  selector: 'app-animal-details',
  templateUrl: './animal-details.component.html',
  styleUrls: ['./animal-details.component.css']
})
export class AnimalDetailsComponent implements OnInit {
  model$: Observable<AnimalDto>;
  constructor(private animalsService: AnimalsService, private aRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.model$ = this.animalsService.getAnimal(this.aRoute.snapshot.params['id']);
  }

  onSubmit() {

  }

  public convertToName(id: number) {
    for (var enumMember in SpeciesEnum) {
      if (parseInt(enumMember) === id) {
        return SpeciesEnum[id];
      }
    }
  }

}
