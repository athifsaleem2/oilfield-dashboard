import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WellService } from '../../services/well';
import { WellStatus } from '../../well.model';

@Component({
  selector: 'app-well-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './well-form.html',
  styleUrl: './well-form.scss',
})
export class WellForm implements OnInit {
  form: FormGroup;
  isEditMode = false;
  wellId: number | null = null;
  statusOptions = [
    { value: WellStatus.Active, label: 'Active' },
    { value: WellStatus.Inactive, label: 'Inactive' },
    { value: WellStatus.Maintenance, label: 'Maintenance' },
  ];
  submitting = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      location: ['', [Validators.required, Validators.maxLength(300)]],
      status: [WellStatus.Active, Validators.required],
      latitude: [0, [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: [0, [Validators.required, Validators.min(-180), Validators.max(180)]],
    });
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      this.wellId = Number(idParam);
      this.wellService.getById(this.wellId).subscribe({
        next: (well) => this.form.patchValue(well),
        error: () => (this.errorMessage = 'Failed to load well.'),
      });
    }
  }

 onSubmit(): void {
  if (this.form.invalid) {
    this.form.markAllAsTouched();
    return;
  }

  this.submitting = true;
  this.errorMessage = null;
  const value = this.form.value;

  if (this.isEditMode && this.wellId) {
    this.wellService.update(this.wellId, { id: this.wellId, ...value }).subscribe({
      next: () => this.router.navigate(['/wells']),
      error: () => {
        this.errorMessage = 'Failed to save well. Please try again.';
        this.submitting = false;
      },
    });
  } else {
    this.wellService.create(value).subscribe({
      next: () => this.router.navigate(['/wells']),
      error: () => {
        this.errorMessage = 'Failed to save well. Please try again.';
        this.submitting = false;
      },
    });
  }
}

  cancel(): void {
    this.router.navigate(['/wells']);
  }
}