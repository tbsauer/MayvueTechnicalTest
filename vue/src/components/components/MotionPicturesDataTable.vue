<template>
  <div id="app" class="container mt-3 mb-5">
    <h2>The Mayvue Motion Pictures Data Table</h2>
    <b-row>
      <b-col>
        <b-button
          v-b-modal.insert-modal
          hover
          variant="outline-dark"
          class="float-end my-2"
          size="sm"
          @click="addClick()"
        >
          <font-awesome-icon icon="fa-solid fa-plus" /> Add A Motion Picture
        </b-button>

        <b-table fixed hover outlined :items="movies" :fields="fields">
          <template v-slot:cell(actions)="data">
            <button v-b-modal.edit-modal @click="editClick(data.item)">
              <font-awesome-icon icon="fa-solid fa-file-pen" />
            </button>
            <button
              class="px-2"
              v-b-popover.focus="'Copy successful!'"
              lg="4"
              hover
            >
              <font-awesome-icon icon="fa-solid fa-copy" />
            </button>
          </template>
        </b-table>

        <b-modal id="insert-modal" title="Add A Film" ref="modal" hide-footer>
          <form ref="form" @submit.stop.prevent="handleSubmitAdd">
            <b-form-group
              id="fieldset-1"
              label="Enter The Film's Title"
              label-for="input-1"
              invalid-feedback="Name is required, under 50 characters"
              valid-feedback="Looks good!"
            >
              <b-form-input
                id="input-1"
                :state="validateState($v.form.name)"
                v-model="$v.form.name.$model"
                trim
              ></b-form-input>
            </b-form-group>
            <b-form-group
              id="fieldset-2"
              description="This is optional"
              label="Enter A Description Of The Film"
              label-for="input-2"
              valid-feedback="Looks good!"
              invalid-feedback="Description can't be over 500 characters"
            >
              <b-form-textarea
                id="input-2"
                rows="6"
                :state="validateState($v.form.description)"
                v-model="$v.form.description.$model"
                trim
              ></b-form-textarea>
            </b-form-group>
            <b-form-group
              id="fieldset-3"
              label="Enter The Year The Film Was Released"
              label-for="input-3"
              invalid-feedback="Please enter a year (Between 1888 and 2023 only)"
            >
              <b-form-input
                id="input-3"
                :state="validateState($v.form.releaseYear)"
                v-model="$v.form.releaseYear.$model"
                trim
              ></b-form-input>
              <div class="btn-group my-4 d-flex align-items-center">
                <b-button hover @click="hideModal" aria-label="Close"
                  ><font-awesome-icon icon="fa-solid fa-arrow-left" /> Go
                  Back</b-button
                >
                <b-button hover variant="success" @click="createClick()">
                  <font-awesome-icon icon="fa-solid fa-clapperboard" /> Submit
                  Film
                </b-button>
              </div>
            </b-form-group>
          </form>
        </b-modal>

        <b-modal id="edit-modal" title="Edit A Film" hide-footer ref="modal">
          <form ref="form">
            <b-form-input
              id="input-id"
              v-model.number="form.id"
              trim
              required
              hidden
            ></b-form-input>
            <b-form-group
              id="fieldset-4"
              label="Enter The Film's Title"
              label-for="input-4"
              invalid-feedback="Name is required, under 50 characters"
              valid-feedback="Looks good!"
            >
              <b-form-input
                id="input-4"
                :state="validateState($v.form.name)"
                v-model="$v.form.name.$model"
                trim
              ></b-form-input>
            </b-form-group>
            <b-form-group
              id="fieldset-5"
              description="This is optional"
              label="Enter A Description Of The Film"
              label-for="input-5"
              class="my-3"
              valid-feedback="Looks good!"
              invalid-feedback="Description can't be over 500 characters"
            >
              <b-form-textarea
                id="input-5"
                rows="6"
                :state="validateState($v.form.description)"
                v-model="$v.form.description.$model"
                trim
              ></b-form-textarea>
            </b-form-group>
            <b-form-group
              id="fieldset-6"
              label="Enter The Year The Film Was Released"
              label-for="input-6"
              invalid-feedback="Please enter a year (Between 1888 and 2023 only)"
            >
              <b-form-input
                id="input-6"
                :state="validateState($v.form.releaseYear)"
                v-model="$v.form.releaseYear.$model"
                trim
              ></b-form-input>
            </b-form-group>
            <div class="btn-group my-4 d-flex align-items-center ">
              <b-button hover @click="hideModal" aria-label="Close"
                ><font-awesome-icon icon="fa-solid fa-arrow-left" /> Go
                Back</b-button
              >
              <b-button hover variant="danger" @click="deleteClick(form.id)">
                <font-awesome-icon icon="fa-solid fa-trash" />Delete Movie
              </b-button>
              <b-button hover variant="success" @click="updateClick(form.id)">
                <font-awesome-icon icon="fa-solid fa-file-pen" /> Save Changes
              </b-button>
            </div>
          </form>
        </b-modal>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  between,
  numeric,
} from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  sortBy: "name",
  sortDesc: false,
  data: () => ({
    message: '',
    copiedText: '',
    movies: [],
    fields: [
      { key: "name", sortable: true },
      { key: "description", sortable: true },
      { key: "releaseYear", sortable: true },
      { key: "actions", sortable: false },
    ],

    form: {
      id: "",
      name: "",
      description: "",
      releaseYear: "",
      createdBy: 1,
    },
  }),

  validations: {
    form: {
      name: {
        required,
        maxLength: maxLength(50),
      },
      description: {
        maxLength: maxLength(500),
      },
      releaseYear: {
        required,
        numeric,
        minLength: minLength(4),
        between: between(1888, 2023),
      },
    },
  },

  methods: {
    async refreshData() {
      axios
        .get("https://localhost:7222/api/motionpictures")
        .then((response) => {
          this.movies = response.data;
        })
        .catch((error) => {
          console.log(error);
          this.errored = true;
        })
        .finally(() => (this.loading = false));
    },
    validateState(item) {
      const { $dirty, $error } = item;
      return $dirty ? !$error : null;
    },
    addClick() {
      this.form = {
        name: "",
        description: "",
        releaseYear: "",
      };
    },
    editClick(movie) {
      this.form = {
        id: movie.id,
        name: movie.name,
        description: movie.description,
        releaseYear: movie.releaseYear,
      };
    },
    removeClick(movie) {
      this.form = {
        id: movie.id,
        name: movie.name,
        description: movie.description,
        releaseYear: movie.releaseYear,
      };
    },
    async createClick() {
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        alert("Please double check the form!");
      } else {
        try {
          const { form } = this;
          await axios.post("https://localhost:7222/api/motionpictures", {
            Name: form.name,
            Description: form.description,
            ReleaseYear: form.releaseYear,
            createdBy: 1,
          });
          this.refreshData();
          alert("You have added a film!");
          this.hideModal();
        } catch {
          // Error Handling
        }
      }
    },
    async updateClick(id) {
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        alert("Please double check the form!");
      } else {
        try {
          const { form } = this;
          await axios.put(`https://localhost:7222/api/motionpictures/${id}`, {
            id: form.id,
            name: form.name,
            description: form.description,
            releaseYear: form.releaseYear,
            createdBy: 1,
          });
          alert("You have edited a film!");
          this.refreshData();
          this.hideModal();
        } catch {
          // Error Handling
        }
      }
    },

    async deleteClick(id) {
      try {
        const { form } = this;
        await axios.delete(`https://localhost:7222/api/motionpictures/${id}`, {
          id: form.id,
        });
        this.refreshData();
        alert("You have deleted a movie!");
        this.hideModal();
      } catch {
        // Error Handling
      }
    },
    handleOkAdd(bvModalEvent) {
      bvModalEvent.preventDefault();
      this.handleSubmitAdd();
      this.createClick();
    },
    handleSubmitAdd() {
      this.$refs.modal.reset();
      this.$nextTick(() => {
        this.$bvModal.hide("modal-prevent-closing");
      });
    },
    hideModal() {
      this.$refs["modal"].hide();
    },
  },
  mounted: function () {
    this.refreshData();
  },
};
</script>
