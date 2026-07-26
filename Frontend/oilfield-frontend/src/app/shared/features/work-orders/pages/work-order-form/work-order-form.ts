import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkOrderService } from '../../services/work-order';
import { WellService } from '../../../wells/services/well';
import { Well } from '../../../wells/well.model';

@Component({
  selector: 'app-work-order-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './work-order-form.html',
  styleUrl: './work-order-form.scss',
})
export class WorkOrderForm implements OnInit {
  form: FormGroup;
  isEditMode = false;
  workOrderId: number | null = null;
  wells: Well[] = [];
  submitting = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private workOrderService: WorkOrderService,
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      wellId: ['', Validators.required],
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      assignedTo: ['', [Validators.required, Validators.maxLength(200)]],
      dueDate: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.wellService.getAll().subscribe({
      next: (wells) => (this.wells = wells),
      error: () => (this.errorMessage = 'Failed to load wells.'),
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      this.workOrderId = Number(idParam);

      this.workOrderService.getAll().subscribe({
        next: (orders) => {
          const order = orders.find((o) => o.id === this.workOrderId);
          if (order) {
            this.form.patchValue({
              wellId: order.wellId,
              title: order.title,
              description: order.description,
              assignedTo: order.assignedTo,
              dueDate: order.dueDate.substring(0, 10),
            });
            this.form.get('wellId')?.disable();
          }
        },
        error: () => (this.errorMessage = 'Failed to load work order.'),
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
    const value = this.form.getRawValue();

    if (this.isEditMode && this.workOrderId) {
      this.workOrderService.update(this.workOrderId, {
        id: this.workOrderId,
        title: value.title,
        description: value.description,
        assignedTo: value.assignedTo,
        status: 0,
        dueDate: value.dueDate,
      }).subscribe({
        next: () => this.router.navigate(['/work-orders']),
        error: () => {
          this.errorMessage = 'Failed to save work order.';
          this.submitting = false;
        },
      });
    } else {
      this.workOrderService.create({
        wellId: value.wellId,
        title: value.title,
        description: value.description,
        assignedTo: value.assignedTo,
        dueDate: value.dueDate,
      }).subscribe({
        next: () => this.router.navigate(['/work-orders']),
        error: () => {
          this.errorMessage = 'Failed to save work order.';
          this.submitting = false;
        },
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/work-orders']);
  }
}